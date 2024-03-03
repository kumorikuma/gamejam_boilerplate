import { useGlobals, useReactiveValue } from "@reactunity/renderer";

import "./index.scss";
import Button from "./button";
import Score from "./score";

export default function Hud(): React.ReactNode {
  const globals = useGlobals();
  const gameLifecycleManager = globals.gameLifecycleManager;
  const score = useReactiveValue(globals.score) as number;

  return (
    <view className="hud">
      <view className="flex-row padding-md">
        <Score value={score} />
        <view className="spacer" />
      </view>
    </view>
  );
}
