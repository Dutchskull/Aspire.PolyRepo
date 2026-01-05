import { env } from "node:process";
import { createServer } from "node:http";
import fetch from "node-fetch";
import express from "express";
import { createTerminus, HealthCheckError } from "@godaddy/terminus";
import { createClient } from "redis";

const app = express();
const port = env.PORT ?? 8080;

const cacheAddress = env["ConnectionStrings__cache"] || "";
const apiServer = env["services__apiservice__http__0"];
const passwordPrefix = ",password=";

let cacheConfig = { url: `redis://${cacheAddress}` };
let cachePasswordIndex = cacheAddress.indexOf(passwordPrefix);

if (cachePasswordIndex > 0) {
  cacheConfig = {
    url: `redis://${cacheAddress.substring(0, cachePasswordIndex)}`,
    password: cacheAddress.substring(
      cachePasswordIndex + passwordPrefix.length
    ),
  };
}

const cache = createClient(cacheConfig);
cache.on("error", (err) => console.error("Redis Client Error", err));

app.set("views", "./views");
app.set("view engine", "pug");

app.get("/", async (req, res) => {
  try {
    let cachedForecasts = await cache.get("forecasts");
    if (cachedForecasts) {
      return res.render("index", { forecasts: JSON.parse(cachedForecasts) });
    }

    let response = await fetch(`${apiServer}/weatherforecast`);
    let forecasts = await response.json();
    await cache.set("forecasts", JSON.stringify(forecasts), { EX: 5 });
    res.render("index", { forecasts: forecasts });
  } catch (err) {
    console.error("Route error:", err);
    res.status(500).send("Internal Server Error");
  }
});

const server = createServer(app);

async function healthCheck() {
  const apiServerHealthAddress = `${apiServer}/health`;
  const response = await fetch(apiServerHealthAddress);
  if (!response.ok) {
    throw new HealthCheckError("Remote API unhealthy", [response.status]);
  }
}

createTerminus(server, {
  signal: "SIGINT",
  healthChecks: {
    "/health": healthCheck,
    "/alive": () => Promise.resolve(),
  },
  onSignal: async () => {
    console.log("Server is starting cleanup");
    await cache.disconnect();
  },
  onShutdown: () => console.log("Cleanup finished, server is shutting down"),
});

async function startServer() {
  try {
    console.log("Connecting to Redis...");
    await cache.connect();

    server.listen(port, () => {
      console.log(`Listening on port ${port}`);
    });
  } catch (err) {
    console.error("Failed to start server:", err);
    process.exit(1);
  }
}

startServer();
