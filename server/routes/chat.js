var express = require('express');
var router = express.Router();
var bodyParser = require('body-parser')
var fs = require('fs');
const { json } = require('body-parser');

// Use bodyParser for POST requests
router.use(bodyParser.urlencoded({ extended: false }));
router.use(bodyParser.json());

// Restore saved chat log
let chat = JSON.parse(fs.readFileSync('./chat.json'));
console.log("\nThis is the saved chat log:");
console.log(chat);

// Prepare dictionary
console.log("Starting to read dictionary...");
const wordsString = fs.readFileSync('./words.txt').toString().toLowerCase();
console.log("Splitting dictionary into words...");
wordArray = wordsString.split('\r\n');

// Remove words with symbols and other shit - yes this code is ugly but i could not group all charachters 
wordArray = wordArray.filter(word => ( word.length > 2 && !word.match(/[|\\/~^:,;?!&%$@*+]/) && !word.match(/[|\\0123456789]/) && !word.match(/[|\\.]/)));

// Sort array by length
wordArray.sort(function(a, b){
    return b.length - a.length;
  });

// Filter sandwich for debugging
console.log('Filtering "sandwich"');
console.log(filterWords('sandwich'));


// Receive POST request with new message from client
router.post('/post', function (req, res) {

    // expected format: { username: '20repsej', messageText: 'This is my message' }
    let message = req.body;

    // log recieved data for debugging
    console.log("Username: " + message.username);
    console.log("Message text: " + message.messageText);

    // Add time to message
    let time = Date.now();
    message.time = time;

    // Remove words from message text
    message.messageText = filterWords(message.messageText);

    // Save new chatlog as json text file
    //console.log(chat.messages);
    chat.messages.push(message);
    //console.log(chat);
    newChatFile = JSON.stringify(chat);
    //console.log(newChatFile);
    try {
        fs.writeFile("./chat.json", newChatFile, () => {});
    } catch (e) {
        console.log("Could not save file: " + e);
    }

    res.end(); // let the client move on with life
});

// Receive POST request from client
// Returns all messages recieved after the specified time
router.post('/get', function (req, res) {

    // Expected format: milliseconds since 1970
    let fromTime = req.body;
    console.log("fromtime: " + fromTime.time);

    // Filter messages
    let response = { messages: []};
    chat.messages.forEach(message => {
        if (message.time > fromTime.time) {
            //console.log("including this: " + message);
            response.messages.push(message);
        } else {
            //console.log("excluded this: " + message);
        }
    });
    //console.log(response);
    res.send(JSON.stringify(response));
});

// Checks if a string makes sense. If it does, unmakesenseit.
function filterWords(str) {
    strippedString = str.replace(/\s+/g, '').toLowerCase();
    let wasIllegal = false;
    wordArray.forEach(
        function (word) {
            if (strippedString.includes(word)) {
                strippedString = strippedString.replace(word, '*');
                wasIllegal = true;
                console.log(word + " was removed!");
            }
        }
    );
    if (wasIllegal) {
        console.log('"' + str + '" was an illegal word. It got replaced with: "' + strippedString + '"');
        return strippedString;
    } else {
        return str;
    }
}


module.exports = router;