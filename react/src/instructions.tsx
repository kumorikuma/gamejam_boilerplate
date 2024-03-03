import { useGlobals, useReactiveValue } from "@reactunity/renderer";

import "./index.scss";

export default function Instructions(): React.ReactNode {
  const globals = useGlobals();

  return (
    <view className="black-bar">
      <view className="content">
        <h1 className="title">Speaker</h1>
        <view className="gradient-rule"></view>
        <p className="message">Text</p>
      </view>
    </view>
  );
}
