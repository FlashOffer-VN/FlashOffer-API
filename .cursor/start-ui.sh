#!/usr/bin/env bash
set -euo pipefail

API_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"

resolve_ui_root() {
  local candidate
  for candidate in \
    "${FLASHOFFER_UI_ROOT:-}" \
    "${API_ROOT}/../FlashOffer-UI" \
    "${API_ROOT}/FlashOffer-UI" \
    "/home/ubuntu/FlashOffer-UI" \
    "/agent/repos/FlashOffer-UI"
  do
    if [ -n "${candidate}" ] && [ -d "${candidate}/package.json" ]; then
      echo "$(cd "${candidate}" && pwd)"
      return 0
    fi
  done
  return 1
}

if ! UI_ROOT="$(resolve_ui_root)"; then
  echo "FlashOffer-UI repo not found. Add it to the multi-repo Cloud Agent environment."
  exit 1
fi

cd "${UI_ROOT}"

if [ "${FLASHOFFER_UI_MODE:-toolchain}" = "docker" ] && [ -f docker-compose.yml ]; then
  echo "Starting FlashOffer-UI via docker compose..."
  docker compose up --build
else
  echo "Starting FlashOffer-UI via npm (toolchain mode)..."
  echo "App: http://localhost:4200"
  npm start -- --host 0.0.0.0 --port 4200
fi
