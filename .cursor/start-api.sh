#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
API_ROOT="$(cd "${SCRIPT_DIR}/.." && pwd)"

cd "${API_ROOT}"

if [ ! -f .env.docker ]; then
  cp .env.docker.example .env.docker
fi

echo "Starting FlashOffer-API stack (SQL Server + API)..."
echo "Health: http://localhost:5000/health"
echo "After first boot, run DB migration — see AGENTS.md"

docker compose up --build
