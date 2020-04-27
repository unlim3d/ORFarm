var TelegramBot = require('node-telegram-bot-api');

var token = '1169870309:AAHLKdGs6HmKzSkGDlRrtz6qnPMSIRUXxmI';
var bot = new TelegramBot(token, {polling: true});
bot.sendMessage(userId, 'Отлично! Я обязательно напомню, если не сдохну :)');
var notes = [];

bot.onText(/напомни (.+) в (.+)/, function (msg, match) {
    var userId = msg.from.id;
    var text = match[1];
    var time = match[2];

    notes.push({ 'uid': userId, 'time': time, 'text': text });

   
});

setInterval(function(){
    for (var i = 0; i < notes.length; i++) {
    const curDate = new Date().getHours() + ':' + new Date().getMinutes();
    if (notes[i]['time'] === curDate) {
      bot.sendMessage(notes[i]['uid'], 'Напоминаю, что вы должны: '+ notes[i]['text'] + ' сейчас.');
      notes.splice(i, 1);
    }
  }
}, 1000);