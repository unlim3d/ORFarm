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

            const button_save = document.createElement('button');
            button_save.setAttribute('class', 'save_btn');
            button_save.innerText = 'Set save path';
            button_save.onclick = () => MakeProgramRequest({name: 'SetRenderFolder', body: {sequence: sequences[i]}});
            td.appendChild(button_save);

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
        div_no_files.innerText = 'You don\'t have any rendered files.';
        table.parentElement.appendChild(div_no_files);
    }
};

const Wait = function (timeout) {
    return new Promise((resolve) => {
        setTimeout(() => {
            resolve();
        }, timeout);
    });
};

const video_test = document.createElement('video');
video_test.autoplay = false;
video_test.muted = true;
video_test.loop = true;
video_test.src = 'http://127.0.0.1:8089/files?filename=Storage_v2_..mov';

const GetVideoFrame = function (video, options = {}){
    const canvas = document.createElement('canvas');
    options.width = options.width || video.videoWidth;
    options.height = video.videoHeight * options.width / video.videoWidth;
    canvas.height = options.height;
    canvas.width = options.width;
    const ctx = canvas.getContext('2d');
    ctx.drawImage(video, 0, 0, canvas.width, canvas.height);
    const img = new Image();
    img.src = canvas.toDataURL();
    return img;
};

const MakeProgramRequest = async function(options){
    const address = 'http://localhost:8090';
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

const GetVideoDuration = function(file){
    return new Promise((resolve) => {
        const video = document.createElement('video');
        video.preload = 'metadata';

        video.onloadedmetadata = function() {
            window.URL.revokeObjectURL(video.src);
            const duration = video.duration;
            resolve(duration);
        }

        video.src = URL.createObjectURL(file);;
    });
};

$(document).ready(async function() {
    //BuildPage();

    $('#seconds_field').change(() =>{
        video_test.currentTime = parseFloat($('#seconds_field').val());
        video_test.play();
    });

    video_test.ontimeupdate = () => {
        const image = GetVideoFrame();
        $('img')[0].src = image.src;
        $('a[download="filename.png"]').attr('href', image.src);
    };

    $('#video_input').change(async function (e){
        const file = $('#video_input')[0].files[0];
        const duration = await GetVideoDuration(file);
        const sliced_file = file.slice(0, file.size / duration / 2);
        const url = URL.createObjectURL(sliced_file);
        const video = document.createElement('video');
        video.autoplay = false;
        video.muted = true;
        video.loop = true;
        video.src = url;
        video.onloadeddata = () => {
            video.currentTime = 0;
            video.onseeked = () => {
                const frame = GetVideoFrame(video, {width: 480});
                $('#frames_container')[0].appendChild(frame);
                URL.revokeObjectURL(url);
            };
        };
    });
});