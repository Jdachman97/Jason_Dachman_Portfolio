gamePattern = [];
userClickedPattern = [];
var userChosenColor = "";
var gameStarted = false;
var level = 0;
buttonColors = ["red", "blue", "green", "yellow"];
var blueSound = new Audio("sounds/blue.mp3");
var greenSound = new Audio("sounds/green.mp3");
var redSound = new Audio("sounds/red.mp3");
var yellowSound = new Audio("sounds/yellow.mp3");
var wrong = new Audio("sounds/wrong.mp3");

$( "body" ).keypress(function() {
    if(!gameStarted) {
        console.log("shmoop");
        nextSequence();
        gameStarted = true;

    }
});

$(".btn").click(function(){
    userChosenColor = this.id;
    userClickedPattern.push(userChosenColor);
    playSound(userChosenColor);
    animatePress(userChosenColor);
    checkAnswer(userClickedPattern.length - 1);
    console.log(userClickedPattern);
});

function nextSequence(){
    userClickedPattern = [];
    var randomNumber = Math.round(Math.random() * 3);
    var randomChosenColor = buttonColors[randomNumber];
    $("#" + randomChosenColor).fadeOut(150).fadeIn(150);
    playSound(randomChosenColor);
    gamePattern.push(randomChosenColor);
    level++;
    $("h1").text("Level " + level);
}

function playSound(name){
    if(name === "blue"){
        blueSound.play();
    }
    else if(name === "green"){
        greenSound.play();
    }
    else if(name === "red") {
        redSound.play();
    }
    else {
        yellowSound.play();
    }
}

function animatePress(currentColor){
    $("#" + currentColor).addClass("pressed");
    setTimeout(function () {
        $("#" + currentColor).removeClass("pressed");
    },100);
}

function checkAnswer(currentLevel) {
   if (gamePattern[currentLevel] === userClickedPattern[currentLevel]) {
      console.log("success");
      if (userClickedPattern.length === gamePattern.length){
        setTimeout(function () {
          nextSequence();
        }, 1000);
      }
    } 
    else {
        wrong.play();
        $("body").addClass("game-over");
        setTimeout(function () {
            $("body").removeClass("game-over");
        },200);
        $("h1").text("Game over, press any key to restart");
        startOver();
    }

}

function startOver() {
    gameStarted = false;
    level = 0;
    gamePattern = [];
}
