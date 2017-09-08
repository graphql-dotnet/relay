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

const version = _.pop();
function getNextVersion() {
  let nextVersion;
  if (['alpha', 'beta', 'rc'].includes(version)) {
    //oldVersion = oldVersion.replace(/(.+[a-z])(\d)/g, (_, a, b) => `${a}.${b}`) // nuget doesn't like semver2
    nextVersion = semver.inc(oldVersion, 'pre', version)
    // nextVersion = nextVersion
    //   .replace(new RegExp(`(${version})\\.(.+)`), (_, a, b) => a + b)
  }
  else if (['major', 'minor', 'patch'].includes(version))
    nextVersion = semver.inc(oldVersion, version)
  else
    nextVersion = version

  return nextVersion
}

async function updateFile(filePath, updater) {
  filePath = resolve(filePath)
  let data = await (filePath.endsWith('.json') ?
    readJson(filePath) :
    readFile(filePath, 'utf8')
  )

  let result = updater(data)
  return typeof result !== 'string' ?
    writeJson(filePath, result, { spaces: 2 })
    writeFile(filePath, result)
}

(async () => {
  let version = getNextVersion()

  await updateFile('package.json', d => Object.assign(d, { version }))


  await updateFile('./src/GraphQL/GraphQL.Relay.csproj', d => d.replace(
    /<VersionPrefix>(.*)<\/VersionPrefix>/,
    `<VersionPrefix>${nextVersion}<\/VersionPrefix>`
  ))
})()
