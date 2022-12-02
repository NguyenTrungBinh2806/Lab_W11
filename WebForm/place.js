const config = { databaseURL: "https://lab-sa-2231-default-rtdb.asia-southeast1.firebasedatabase.app" };
firebase.initializeApp(config); 
const dbRef = firebase.database().ref(); 

function page_load() {
    getAll();
}

function lnkID_Click(code) {
    getDetails(code);
}

function btnSearch_Click() {
    var keyword = document.getElementById("txtKeyword").value.trim();
    if (keyword.length > 0){
        search(keyword);
    }else{
        getAll();
    }
}

function btnAdd_Click() {
    var newPlace = {
        Code: document.getElementById("txtCode").value,
        Name: document.getElementById("txtName").value,
        Address: document.getElementById("txtAddress").value,
        Description: document.getElementById("txtDescription").value,
        Nation: document.getElementById("txtNation").value,
        Rate: document.getElementById("txtRate").value,
        Image: document.getElementById("txtImage").value,   
    };
    addNew(newPlace);
}

function btnUpdate_Click() {
    var newPlace = {
        Code: document.getElementById("txtCode").value,
        Name: document.getElementById("txtName").value,
        Address: document.getElementById("txtAddress").value,
        Description: document.getElementById("txtDescription").value,
        Nation: document.getElementById("txtNation").value,
        Rate: document.getElementById("txtRate").value,
        Image: document.getElementById("txtImage").value,   
    };
    update(newPlace);
}

function btnDelete_Click() {
    if(confirm("Are you sure to delete this place?")){
        var code = document.getElementById("txtCode").value;
        deletePlace(code);
    }
}

// fetch SOAP methods

// fetch API methods
function getAll(){
    dbRef.child("places").on("value", (snapshot) => {
        var places = [];
        snapshot.forEach((child) => {
            var place = child.val();
            places.push(place);
        });
        renderPlaceList(places);
    });
}

function getDetails(code){
    dbRef.child("places").once("value", (snapshot) => {
        snapshot.forEach((child) => {
            var place = child.val();
            if(place.Code == code){
                renderPlaceDetails(place);
            }
        });
    });
}

function search(keyword){
    dbRef.child("places").once("value", (snapshot) => {
        var places = [];
        snapshot.forEach((child) => {
            var place = child.val();
            if(place.Name.toLowerCase().includes(keyword.toLowerCase())){
                places.push(place);
            }
        });
        renderPlaceList(places);
    });
}

function addNew(newPlace){
    dbRef.child("places/P" + newPlace.Code).set(newPlace);
}

function update(newPlace){
    dbRef.child("places").once("value", (snapshot) => {
        snapshot.forEach((child) => {
            var place = child.val();
            if(place.Code == newPlace.Code){
                var key = child.key;
                dbRef.child("places").child(key).set(newPlace);
            }
        });
    });
}

function deletePlace(code){
    dbRef.child("places").once("value", (snapshot) => {
        snapshot.forEach((child) => {
            var place = child.val();
            if(place.Code == code){
                var key = child.key;
                dbRef.child("places").child(key).remove();
            }
        });
    });
}





// helper methods
function renderPlaceList(places){
    var rows = "";
    for(var place of places){
        rows += "<tr onclick='lnkID_Click(" + place.Code + ")' style='cursor: pointer'>"; 
        rows += "<td>" + place.Code + "</td>";
        rows += "<td>" + place.Name + "</td>";
        rows += "<td>" + place.Address + "</td>";
        rows += "<td>" + place.Description + "</td>";
        rows += "<td>" + place.Nation + "</td>";
        rows += "<td>" + place.Rate + "</td>";
        rows += "<td>" + place.Image + "</td>";
        rows += "</tr>";      
    }
    var header = "<tr style='background-color: #CCCCCC'><th>Code</th><th>Name</th><th>Address</th><th>Description</th><th>Nation</th><th>Rate</th><th>Image</th></tr>";
    document.getElementById("lstPlace").innerHTML = header + rows;
}

function renderPlaceDetails(place){
    document.getElementById("txtCode").value = place.Code;
    document.getElementById("txtName").value = place.Name;
    document.getElementById("txtAddress").value = place.Address;
    document.getElementById("txtDescription").value = place.Description;
    document.getElementById("txtNation").value = place.Nation;
    document.getElementById("txtRate").value = place.Rate;
    document.getElementById("txtImage").value = place.Image;
}

function clearTextboxes(){
    document.getElementById("txtCode").value = "";
    document.getElementById("txtName").value = "";
    document.getElementById("txtAddress").value = "";
    document.getElementById("txtDescription").value = "";
    document.getElementById("txtNation").value = "";
    document.getElementById("txtRate").value = "";
    document.getElementById("txtImage").value = "";
}