const displayNoneValue = "d-none";
const STRING_EMPTY = "";


function show(element) {
    element?.classList.remove(displayNoneValue);
}

function hide(element) {
    element?.classList.add(displayNoneValue);
}