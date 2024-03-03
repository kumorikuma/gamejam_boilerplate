import { useGlobals, useReactiveValue } from "@reactunity/renderer";
import "./index.scss";

import Button from "./button";

interface LeaderboardScores {
  LatestScore: number;
  LatestRanking: number;
  BestScore: number;
  BestRanking: number;
  GlobalBestScore: number;
  NumberScores: number;
}

export default function Leaderboard() {
  const globals = useGlobals();
  const gameLifecycleManager = globals.gameLifecycleManager;
  const leaderboardScores = useReactiveValue(
    globals.leaderboardScores
  ) as LeaderboardScores;
  const {
    LatestScore,
    LatestRanking,
    BestScore,
    BestRanking,
    GlobalBestScore,
    NumberScores,
  } = leaderboardScores;

  return (
    <view className="leaderboard">
      <view className="title">Leaderboard</view>
      <view className="content">
        <view className="score">{`My Best Score:  ${
          BestScore == 0 ? "N/A" : BestScore
        }`}</view>
        <view className="score">{`My Best Ranking:  #${
          BestRanking == 0 ? "N/A" : BestRanking
        } out of ${NumberScores} players`}</view>
        <view className="score">{`World Top Score:  ${
          GlobalBestScore == 0 ? "N/A" : GlobalBestScore
        }`}</view>
      </view>
      <Button
        text="Main Menu"
        onClick={() => {
          gameLifecycleManager.ReturnToMainMenu();
        }}
      />
    </view>
  );
}
