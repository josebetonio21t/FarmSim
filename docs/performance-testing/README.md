# Backend performance testing

Target: `http://localhost:5000`

Tested routes:

- `GET /api/sensors/current`
- `GET /health`

The default `load` profile uses 50 virtual users and the assignment thresholds:

- `http_req_failed`: rate below 1%
- `http_req_duration`: p95 below 200 ms
- `checks`: more than 99% successful

Start the backend with `run-backend.cmd`, then use `run-k6.cmd -Profile load`. Use only an actual saved run when completing the report; do not invent values or claim a database/cloud deployment that was not tested.
