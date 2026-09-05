// Dev-server proxy for Aspire orchestration.
//
// Aspire injects the API endpoint into this npm process as environment
// variables (see .WithReference(apiService) in Aspire.AppHost/Program.cs):
//   services__api__https__0  -> https://localhost:<port>
//   services__api__http__0   -> http://localhost:<port>
//
// We forward all /api requests to that endpoint so the browser only ever
// talks to the Angular dev server (same-origin, no CORS needed).

const target =
  process.env['services__api__https__0'] ||
  process.env['services__api__http__0'] ||
  'https://localhost:7154';

module.exports = {
  '/api': {
    target,
    secure: false, // accept the dev HTTPS cert
    changeOrigin: true,
    logLevel: 'debug',
  },
};
