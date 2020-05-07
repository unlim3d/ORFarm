

import fs = require('fs');
import * as path from "path";
const Export = function (RenderPath: string )
{
    let data = JSON.stringify(RenderPath);
    fs.writeFileSync( path.join(RenderPath,".json"), data);


}