import { useGlobals, useReactiveValue } from "@reactunity/renderer";

import Button from "./button";
import "./index.scss";

export default function MainMenu(): React.ReactNode {
  const globals = useGlobals();
  const gameLifecycleManager = globals.gameLifecycleManager;

  return (
    <view className="main-menu">
      <view className="content">
        <Button
          text="Start Game"
          onClick={() => {
            gameLifecycleManager.StartGame(0, true);
          }}
        />
        <Button
          text="Leaderboard"
          onClick={() => {
            gameLifecycleManager.ToLeaderboard();
          }}
        />
      </view>
    </view>
  );
}
