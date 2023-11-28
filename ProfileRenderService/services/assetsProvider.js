const fs = require('fs');
const path = require('path');
const manifest = fs.readFileSync(path.join(__dirname, '../dist/static/manifest.json'), 'utf-8');

module.exports = {
    getAssets: () => JSON.parse(manifest)
}