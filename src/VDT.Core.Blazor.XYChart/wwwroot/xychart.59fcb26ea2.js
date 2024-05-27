const handlers = {};

function register(dotNetObjectReference) {
    handlers[dotNetObjectReference] = function () {
        dotNetObjectReference.invokeMethodAsync('StateHasChanged');
    }

    window.addEventListener('resize', handlers[dotNetObjectReference]);
}

function unregister(dotNetObjectReference) {
    if (handlers[dotNetObjectReference]) {
        window.removeEventListener('resize', handlers[dotNetObjectReference]);
    }

    delete handlers[dotNetObjectReference];
}

// TODO integrate boundingboxprovider ?
function getAvailableWidth(element) {
    if (element && element.parentElement) {
        const parentWidth = element.parentElement.getBoundingClientRect().width;

        const parentComputedStyle = getComputedStyle(element.parentElement);
        const parentBorder = getSize(parentComputedStyle, "border-left-width") + getSize(parentComputedStyle, "border-right-width");
        const parentPadding = getSize(parentComputedStyle, "padding-left") + getSize(parentComputedStyle, "padding-right");

        const elementComputedStyle = getComputedStyle(element);
        const elementMargin = getSize(elementComputedStyle, "margin-left") + getSize(elementComputedStyle, "margin-right");
        const elementBorder = getSize(elementComputedStyle, "border-left-width") + getSize(elementComputedStyle, "border-right-width");

        return parentWidth - parentBorder - parentPadding - elementMargin - elementBorder;
    }
}

function getSize(computedStyle, property) {
    const size = +computedStyle.getPropertyValue(property).slice(0, -2);
    
    return isNaN(size) ? 0 : size;
}

// TODO height

export { register, unregister, getAvailableWidth };
