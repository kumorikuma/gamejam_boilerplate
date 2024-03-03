import { useGlobals, useReactiveValue } from "@reactunity/renderer";

import Button from "./button";
import "./index.scss";

export default function PauseMenu(): React.ReactNode {
  const globals = useGlobals();
  const gameLifecycleManager = globals.gameLifecycleManager;

  return (
    <view className="pause-menu">
      <view className="black-bar">
        <view className="container flex-row align-center spacer">
          <view className="text">Paused</view>
          <view className="spacer" />
          <Button
            text="Main Menu"
            onClick={() => {
              gameLifecycleManager.ReturnToMainMenu();
            }}
          />
          <Button
            text="Continue"
            onClick={() => {
              gameLifecycleManager.UnpauseGame();
            }}
          />
        </view>
        <view className="gradient-rule"></view>
      </view>
    </view>
  );
}
