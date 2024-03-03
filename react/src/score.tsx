import React from "react";
import "./index.scss";

type ScoreProps = {
    value: number;
};

// I had to put the score and score text in separate spans because Unity is dumb
export const Score = (props: ScoreProps) => (
    <>
        <span class="score text">Score: </span>
        <span class="score">{props.value}</span>
    </>
)

export default Score;
