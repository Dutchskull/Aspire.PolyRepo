import { useEffect, useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'

type Forecast = {
  date: string,
  temperatureC: number;
  temperatureF: number;
  summary: string;
}

function App() {
  const [forecasts, setForecasts] = useState<Forecast[]>([]);

  const weatherApi = import.meta.env.VITE_WEATHER_API;

  console.log(weatherApi);

  useEffect(() => {
    const fetchData = async (weatherApi: string) => {
      const weather = await fetch(weatherApi);
      console.log(weather);

      const weatherJson = await weather.json();
      console.log(weatherJson);

      setForecasts(weatherJson);
    };

    fetchData(weatherApi);
  }, [weatherApi]);

  return (
    <>
      <div>
        <a href="https://vite.dev" target="_blank">
          <img src={viteLogo} className="logo" alt="Vite logo" />
        </a>
        <a href="https://react.dev" target="_blank">
          <img src={reactLogo} className="logo react" alt="React logo" />
        </a>
      </div>
      <h1>Vite + React</h1>
      <h2>Mode: {import.meta.env.VITE_ENVIRONMENT_MODE}</h2>
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
    </>
  )
}

export default App
