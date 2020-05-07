const utils = require('./base/utils');

const Start = async function (){
    const promises = [];
    promises.push(await utils.Exec('npm list express || npm install express --save'));
    promises.push(await utils.Exec('npm list cookie-parser|| npm install cookie-parser --save'));
    promises.push(await utils.Exec('npm list ejs || npm install ejs --save'));
    await Promise.all(promises);
    console.log("Checking modules Finished.");
    const routing = require('./web_server');
};

Start();