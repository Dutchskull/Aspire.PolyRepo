import { useEffect, useState } from "react";

import logo from "./logo.svg";
import "./App.css";

function App({ weatherApi }) {
  const [forecasts, setForecasts] = useState([]);

  console.log(weatherApi);

  const fetchData = async (weatherApi) => {
    const weather = await fetch(weatherApi);
    console.log(weather);

    const weatherJson = await weather.json();
    console.log(weatherJson);

    setForecasts(weatherJson);
  };

  useEffect(() => {
    fetchData(weatherApi);
  }, [weatherApi]);

  return (
    <div className="App">
      <header className="App-header">
        <img
          src={logo}
          className="App-logo"
          alt="logo"
          title={forecasts?.length}
        />
        <table>
          <thead>
            <tr>
              <th>Date</th>
              <th>Temp. (C)</th>
              <th>Temp. (F)</th>
              <th>Summary</th>
            </tr>
          </thead>
          <tbody>
            {(
              forecasts ?? [
                {
                  date: "N/A",
                  temperatureC: "",
                  temperatureF: "",
                  summary: "No forecasts",
                },
              ]
            ).map((w) => {
              return (
                <tr>
                  <td>{w.date}</td>
                  <td>{w.temperatureC}</td>
                  <td>{w.temperatureF}</td>
                  <td>{w.summary}</td>
                </tr>
              );
            })}
          </tbody>
        </table>
      </header>
    </div>
  );
}

export default App;
