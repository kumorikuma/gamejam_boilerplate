import "./index.scss";

export default function Hud(): React.ReactNode {
  return (
    <view className="hud">
      <view className="flex-row padding-md">
        <view>HUD</view>
        <view className="spacer" />
      </view>
    </view>
  );
}
