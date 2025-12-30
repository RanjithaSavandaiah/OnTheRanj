const http = require('http');
const fs = require('fs');
const path = require('path');

const server = http.createServer((req, res) => {
  console.log('Request:', req.url);
  if (req.url === '/') {
    res.writeHead(200, { 'Content-Type': 'text/html' });
    res.end('<html><body><h1>Test Server Works</h1><p>Port 4200 is accessible</p></body></html>');
  } else {
    res.writeHead(404);
    res.end('Not found');
  }
});

server.listen(4201, () => {
  console.log('Test server running at http://localhost:4201');
});
