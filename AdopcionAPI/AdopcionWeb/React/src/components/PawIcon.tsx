interface PawIconProps {
  color?: string;
  size?: number;
  className?: string;
}

export function PawIcon({ color = "currentColor", size = 22, className }: PawIconProps) {
  return (
    <svg
      width={size}
      height={size}
      viewBox="0 0 24 24"
      fill="none"
      xmlns="http://www.w3.org/2000/svg"
      className={className}
      aria-hidden="true"
    >
      <path
        d="M19.58 15.72C18.99 12.19 15.6 9.32 12.02 9.32C8.13 9.32 4.71 12.47 4.38 16.35C4.25 17.85 4.73 19.27 5.72 20.34C6.7 21.41 8.08 22 9.58 22H14.26C15.95 22 17.43 21.34 18.44 20.15C19.45 18.96 19.85 17.38 19.58 15.72Z"
        fill={color}
      />
      <circle cx="10.78" cy="4.93" r="2.93" fill={color} />
      <circle cx="17.44" cy="6.59" r="2.44" fill={color} />
      <circle cx="21.05" cy="10.98" r="1.95" fill={color} />
      <circle cx="4.44" cy="8.54" r="2.44" fill={color} />
    </svg>
  );
}
