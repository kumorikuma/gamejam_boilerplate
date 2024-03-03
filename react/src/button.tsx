import React from "react";
import "./index.scss";

type ButtonProps = {
  text?: string;
  onClick?: () => void;
  isDisabled?: boolean;
  className?: string;
};

export default function Button({
  text,
  onClick,
  isDisabled = false,
  className = "",
}: ButtonProps): React.ReactElement {
  return (
    <button
      className={`button ${className} ${isDisabled ? "disabled" : ""}`}
      onClick={() => {
        onClick && onClick();
      }}
    >
      {text}
    </button>
  );
}
