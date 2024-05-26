const handlers = {};

function register(dotNetObjectReference, element) {
    handler[dotNetObjectReference] = function (e) {
        dotNetObjectReference.invokeMethodAsync('InvokeResize', getAvailableSize(element));
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
function getAvailableSize(element) {
    if (element && element.parentElement) {
        // take into account margin/border/padding since getBoundingClientRect includes parent border/padding and child margin
        const bbox = element.parentElement.getBoundingClientRect();
        return {
            width: bbox.width,
            height: bbox.height
        };
    }
}

export { register, unregister, getSize };
