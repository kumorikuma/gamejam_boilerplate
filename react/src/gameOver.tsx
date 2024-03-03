import { useGlobals } from "@reactunity/renderer";

import Button from "./button";
import "./index.scss";

export default function GameOver(): React.ReactNode {
  const globals = useGlobals();
  const gameLifecycleManager = globals.gameLifecycleManager;

  return (
    <view className="gameover">
      <view className="title">Game Over</view>
      <Button
        className="mainMenuButton"
        text="Main Menu"
        onClick={() => {
          gameLifecycleManager.ReturnToMainMenu();
        }}
      />
    </view>
  );
}
