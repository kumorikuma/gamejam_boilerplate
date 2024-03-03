import { useGlobals } from "@reactunity/renderer";

import Button from "./button";
import "./index.scss";

export default function MainMenu(): React.ReactNode {
  const globals = useGlobals();
  const gameLifecycleManager = globals.gameLifecycleManager;

  return (
    <view className="main-menu">
      <view className="content">
        <view className="title">Game Template</view>
        <Button
          text="Start Game"
          onClick={() => {
            gameLifecycleManager.StartGame();
          }}
        />
      </view>
    </view>
  );
}
