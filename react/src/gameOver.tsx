import { useGlobals, useReactiveValue } from "@reactunity/renderer";

import Button from "./button";
import Score from "./score";
import "./index.scss";

interface LeaderboardScores {
  LatestScore: number;
  LatestRanking: number;
  BestScore: number;
  BestRanking: number;
  GlobalBestScore: number;
  NumberScores: number;
}

export default function GameOver(): React.ReactNode {
  const globals = useGlobals();
  const gameLifecycleManager = globals.gameLifecycleManager;
  const status = useReactiveValue(globals.status);
  const canContinue = useReactiveValue(globals.canContinue);

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

  var title = status;
  if (canContinue) {
    title = "Let's keep dreaming~";
  }

  const intermediateScreen = (
    <>
      <Button
        text="Continue"
        onClick={() => {
          gameLifecycleManager.ToNextLevel();
        }}
      />
      <view className="score flex-row padding-md">
        <Score value={globals.score.Value} />
      </view>
    </>
  );

  const endingScreen = (
    <>
      <view className="score latest">{`Score:  ${LatestScore}`}</view>
      <view className="score ranking">{`Ranking:  #${LatestRanking} out of ${NumberScores} players`}</view>
      <view className="score">{`My Best Score:  ${
        BestScore == 0 ? "N/A" : BestScore
      }`}</view>
      <view className="score">{`My Best Ranking:  #${
        BestRanking == 0 ? "N/A" : BestRanking
      }`}</view>
      <view className="score">{`World Top Score:  ${
        GlobalBestScore == 0 ? "N/A" : GlobalBestScore
      }`}</view>
      <Button
        className="mainMenuButton"
        text="Main Menu"
        onClick={() => {
          gameLifecycleManager.ReturnToMainMenu();
        }}
      />
    </>
  );

  return (
    <view className="gameover">
      <view className="title">{title}</view>
      <view className="content">
        {canContinue && intermediateScreen}
        {!canContinue && endingScreen}
      </view>
    </view>
  );
}
