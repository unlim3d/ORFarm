var msg = require('./JobExport.js');

const BuildPage = function () {
    let files = JSON.parse(document.getElementById('files').textContent);

    const table = document.getElementById('table');

    const sequences = files[1][0];
    if (!files[0] && sequences.length > 0){
        files = files[1];
        let tr = document.createElement('tr');
        for (let i = 0; i < sequences.length; i++){
            const th = document.createElement('th');
            th.innerText = sequences[i];
            tr.appendChild(th);
        }
        table.appendChild(tr);

        const videos = files[1];
        tr = document.createElement('tr');
        for (let i = 0; i < videos.length; i++){
            const file = videos[i];

            const td = document.createElement('td');
            if (file) {
                td.setAttribute('alt', file.filename);
                td.setAttribute('title', file.filename);
                const video = document.createElement('video');
                video.setAttribute('controls', 'controls');
                video.setAttribute('loop', 'loop');
                const source = document.createElement('source');
                source.setAttribute('src', '/files?filename=' + file.filename + '.' + file.format);
                video.appendChild(source);
                td.appendChild(video);
            }

            const text_field_path = document.createElement('input');
            text_field_path.setAttribute('type', 'text');
            text_field_path.setAttribute('placeholder', 'Enter path...');
            text_field_path.setAttribute('class', 'text path');
            td.appendChild(text_field_path);

            const button_save = document.createElement('button');
            button_save.setAttribute('class', 'save_btn');
            button_save.innerText = 'Папка для сбора финальных рендеров со слоями';
            button_save.onclick = () =>{
                MakeProgramRequest({name: 'SetRenderFolder', body: {sequence: sequences[i], path: text_field_path.value}});
                JobExport(text_field_path.value);
            }
            td.appendChild(button_save);
 

            const text_field_collect_path = document.createElement('input');
            text_field_collect_path.setAttribute('type', 'text');
            text_field_collect_path.setAttribute('placeholder', 'Куда складировать результаты...');
            text_field_collect_path.setAttribute('class', 'text path');
            td.appendChild(text_field_collect_path);


            const button_save_collect_path = document.createElement('button');
            button_save_collect_path.setAttribute('class', 'save_btn');
            button_save_collect_path.innerText = 'Применить';
            button_save_collect_path.onclick = () => MakeProgramRequest({name: 'SetCollectFolder', body: {sequence: sequences[i], path:  text_field_collect_path.value}});
            td.appendChild(button_save_collect_path);


            tr.appendChild(td);
            table.appendChild(tr);
        }

        for (let i = 2; i < files.length; i++) {
            const tr = document.createElement('tr');
            for (let j = 0; j < files[i].length; j++){
                const file = files[i][j];

                const td = document.createElement('td');
                const img = document.createElement('img');

                td.appendChild(img);
                if (file && file.type === 'frame') {
                    td.setAttribute('alt', file.filename);
                    td.setAttribute('title', file.filename);

                    img.setAttribute('src', '/files?filename=' + file.filename + '.' + file.format);

                    if (file.data){
                        const fields = ['Slave', 'Renderer', 'Frame', 'RenderTime', 'ServerPreviewFileName'];

                        for (let k = 0; k < fields.length; k++){
                            const val = file.data[fields[k]];
                            const div = document.createElement('div');
                            div.innerText = fields[k] + ': ' + val;
                            div.setAttribute('style', 'top: ' + ((k + .5) * 1.5) + 'em');
                            td.appendChild(div);
                        }
                    }
                }else if (file && file.type === 'skipped'){
                    img.setAttribute('src', 'img/no_preview.svg');
                }

                if (!file || !file.data){
                    const div = document.createElement('div');
                    div.innerText = 'No Such Info';
                    div.setAttribute('style', 'top: 1.5em');
                    td.appendChild(div);
                }

                tr.appendChild(td);
                table.appendChild(tr);
            }
        }
    }else{

         const div_no_files = document.createElement('div');
         div_no_files.setAttribute('class', 'attention no_files');
         div_no_files.innerText = 'Добавьте новый путь рендера файлов для сбора информации.';
         table.parentElement.appendChild(div_no_files);
    }
};

const MakeProgramRequest = async function(options){
    const address = window.location.href.split(':').slice(0, 2).join(':') + ':8090';
    const xhr = new XMLHttpRequest();

    options.method = options.method || 'POST';
    options.name = options.name || 'SetRenderFolder';
    options.body = options.body || {};

    xhr.onreadystatechange = () => {
        if (xhr.readyState === 4 && xhr.status === 200) {
            alert(xhr.response);
        }
    };

    xhr.open(options.method, address + '/' + options.name);
    xhr.setRequestHeader('Content-Type', 'application/json');
    xhr.send(JSON.stringify(options.body));
};

$(document).ready(() => {
    BuildPage();
});