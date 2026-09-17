import http from 'k6/http';
import { check, group, sleep } from 'k6';

const profiles = {
  smoke: {
    vus: 1,
    iterations: 5,
  },
  load: {
    stages: [
      { duration: '1m', target: 50 },
      { duration: '3m', target: 50 },
      { duration: '1m', target: 0 },
    ],
  },
};

const selectedProfile = __ENV.K6_PROFILE || 'load';

if (!profiles[selectedProfile]) {
  throw new Error(`Unknown K6_PROFILE '${selectedProfile}'. Use 'smoke' or 'load'.`);
}

export const options = {
  ...profiles[selectedProfile],
  thresholds: {
    checks: ['rate>0.99'],
    http_req_failed: ['rate<0.01'],
    http_req_duration: ['p(95)<200'],
  },
  summaryTrendStats: ['avg', 'med', 'p(90)', 'p(95)', 'max'],
};

const BASE_URL = (__ENV.BASE_URL || 'http://localhost:5000').replace(/\/$/, '');

export default function () {
  group('sensor reading', () => {
    const response = http.get(`${BASE_URL}/api/sensors/current`, {
      tags: { endpoint: 'sensors-current' },
    });

    check(response, {
      'sensor status is 200': (result) => result.status === 200,
      'sensor body includes pH': (result) => {
        if (result.status !== 200) return false;
        const body = result.json();
        return body.ph >= 0 && body.ph <= 14;
      },
    });
  });

  group('health', () => {
    const response = http.get(`${BASE_URL}/health`, {
      tags: { endpoint: 'health' },
    });

    check(response, {
      'health status is 200': (result) => result.status === 200,
      'service reports healthy': (result) =>
        result.status === 200 && result.json('status') === 'healthy',
    });
  });

  sleep(1);
}
