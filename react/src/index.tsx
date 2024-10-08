import { useGlobals, useReactiveValue } from "@reactunity/renderer";
import { render } from "@reactunity/renderer";
import "./index.scss";

import { MemoryRouter, Route, Routes, useNavigate } from "react-router";
import { useEffect } from "react";

import MainMenu from "./mainMenu";
import PauseMenu from "./pauseMenu";
import Hud from "./hud";
import GameOver from "./gameOver";
import Debug from "./debug";

export default function App() {
  const globals = useGlobals();
  const route = useReactiveValue(globals.route);

  const navigate = useNavigate();

  useEffect(() => {
    navigate(route);
  }, [route, navigate]);

  return (
    <>
      <Routes>
        <Route path="/" element={<view />} />
        <Route path="/mainMenu" element={<MainMenu />} />
        <Route path="/pauseMenu" element={<PauseMenu />} />
        <Route path="/hud" element={<Hud />} />
        <Route path="/gameOver" element={<GameOver />} />
      </Routes>
      <Debug />
    </>
  );
}

render(
  <MemoryRouter>
    <App />
  </MemoryRouter>
);
