const displayNoneValue = "d-none";

function show(element) {
    element?.classList.remove(displayNoneValue);
}

function hide(element) {
    element?.classList.add(displayNoneValue);
}