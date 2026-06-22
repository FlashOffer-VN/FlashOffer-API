#!/usr/bin/env bash
# Idempotent install script for Cursor Cloud Agent snapshots.
# Runs from FlashOffer-API repo root.

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
API_ROOT="$(cd "${SCRIPT_DIR}/.." && pwd)"

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

echo "==> Configure Docker daemon for DinD"
sudo mkdir -p /etc/docker
sudo cp "${SCRIPT_DIR}/daemon.json" /etc/docker/daemon.json
sudo service docker start || true

echo "==> FlashOffer-API: prepare env + restore"
cd "${API_ROOT}"
if [ ! -f .env.docker ] && [ -f .env.docker.example ]; then
  cp .env.docker.example .env.docker
fi

dotnet restore
dotnet build --no-restore -c Release

echo "==> FlashOffer-UI: install npm deps (if sibling repo exists)"
if UI_ROOT="$(resolve_ui_root)"; then
  echo "    UI repo: ${UI_ROOT}"
  cd "${UI_ROOT}"
  npm ci --legacy-peer-deps
else
  echo "    UI repo not found — skip npm ci (single-repo mode or add FlashOffer-UI to multi-repo env)"
fi

echo "==> Install complete"
