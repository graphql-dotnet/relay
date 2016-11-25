const { blue, red, green } = require('chalk');
const { writeFileSync } = require('fs');
const { resolve, join } = require('path');
const yargs = require('yargs');
const get = require('lodash/get');
const set = require('lodash/set');
const glob = require('glob');
const semver = require('semver');

let { _, path, preid } = yargs
  .help()
  .usage('$0 <source> [args]')
  .option('path', { string: true, required: true, 'default': 'version' })
  .option('preid', { string: true })
  .argv;

const version = _.pop();
const files = _.reduce((arr, pattern) => arr.concat(glob.sync(pattern)), []);

files.forEach(file => {
  const json = require(join(process.cwd(), file))
  const oldVersion = get(json, path);

  let nextVersion;
  if (preid)
    nextVersion = semver.inc(oldVersion, 'pre', preid)
  else if (['major', 'minor', 'patch'].includes(version))
    nextVersion = semver.inc(oldVersion, version)
  else
    nextVersion = version

  console.log('updating ' + blue(file) + ' to version to: ' + blue(nextVersion) + '\n');
  set(json, path, nextVersion);
  writeFileSync(file, JSON.stringify(json, null, 2) + '\n', 'utf8');
})
