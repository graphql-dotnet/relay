const { blue, red, green } = require('chalk');
const { readJson, readFile, writeFile, writeJson } = require('fs-extra');
const { resolve, join } = require('path');
const yargs = require('yargs');
const get = require('lodash/get');
const set = require('lodash/set');
const glob = require('glob');
const semver = require('semver');

let { _ } = yargs
  .help()
  .usage('$0 <version>')
  .argv;


(async () => {
  let version = getNextVersion()

  await updateFile('package.json', d => Object.assign(d, { version }))

  await updateFile('./src/GraphQL.Relay/GraphQL.Relay.csproj', d => d.replace(
    /<VersionPrefix>(.*)<\/VersionPrefix>/,
    `<VersionPrefix>${version}<\/VersionPrefix>`
  ))
})()


const version = _.pop();
function getNextVersion() {
  let currentVersion = require('../package.json').version
  let [nextVersion] = _;

  if (['alpha', 'beta', 'rc'].includes(nextVersion))
    nextVersion = semver.inc(currentVersion, 'pre', nextVersion)

  else if (['major', 'minor', 'patch'].includes(nextVersion))
    nextVersion = semver.inc(currentVersion, nextVersion)

  return nextVersion
}

async function updateFile(filePath, updater) {
  let target = resolve(filePath)
  let data = await (filePath.endsWith('.json') ?
    readJson(target) :
    readFile(target, 'utf8')
  )
  console.log(`Updating ${filePath}`)
  let result = updater(data)
  return typeof result !== 'string' ?
    writeJson(target, result, { spaces: 2 }) :
    writeFile(target, result)
}

