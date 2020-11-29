var express = require('express');
var router = express.Router();
var bodyParser = require('body-parser')
var fs = require('fs');
var chat = require('./chat.json');

// Use bodyParser for POST requests
router.use(bodyParser.urlencoded({extended: false}));
router.use(bodyParser.json());

// Receive POST request from client
router.post('/post', function (req, res){


    // expected format: { username: '20repsej', messageText: 'This is my message' }
    let message = req.body;

    // log recieved data for debugging
    console.log("This is what I recieved: " + message);
    console.log("Username: " + message.username);
    console.log("Message text: " + message.messageText);


    chat.messages.push(message);

    fs.writeFile("chat.json", chat, x => x);

    res.send(); // let the client move on with life
});

// Send old messages to client
router.get('/getFirst', function (req, res, next) {

    res.send(chat);
});

router.get('/set', function (req, res, next) {
    let username = req.query.username;
    let messageText = req.query.messageText;

    let message = JSON.parse(username, messageText);

    chat.messages.push(message);

    res.render('index', { title: 'Get Chat' });
});



function isNotWord(message) {
    let messageArr = message.split(/[ ]+/);
    console.log(messageArr);
    let cleanMessageArr = messageArr.map(m => m.replace(/[^\W\s]/gi, ''));

    let testobj = { 'time': '2020', 'text': 'mitt fina meddelande att skicka' };
    fs.appendFile('chat.json', ',\n' + JSON.stringify(testobj), function upd() {
        console.log("TODO error if error");
    });
}

module.exports = router;