import { useGlobals, useReactiveValue } from "@reactunity/renderer";

import "./index.scss";
import Button from "./button";

export default function Debug(): React.ReactNode {
  const globals = useGlobals();
  const route = useReactiveValue(globals.route);
  const gameState = useReactiveValue(globals.debugGameState);
  const gameLifecycleManager = globals.gameLifecycleManager;
  const isDebugModeEnabled = useReactiveValue(globals.debugModeEnabled);

  return (
    isDebugModeEnabled && (
      <view className="debug-ui">
        <view className="spacer" />
        <view className="footer">
          <view className="flex-column">
            <view className="text">Debug</view>
            <view className="text">{`UI Route: ${route}`}</view>
            <view className="text">{`Game State: ${gameState}`}</view>
            {gameState === "GamePaused" && (
              <Button
                text="End Game"
                onClick={() => {
                  gameLifecycleManager.EndGame();
                }}
              />
            )}
          </view>
        </view>
      </view>
    )
  );
}
