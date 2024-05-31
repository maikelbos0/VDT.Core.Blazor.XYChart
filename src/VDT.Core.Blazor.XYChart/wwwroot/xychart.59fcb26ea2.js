const svgNamespace = "http://www.w3.org/2000/svg";
const charts = {};

function register(dotNetObjectReference) {
    const chart = {
        ...CreateSvgElement(),
        eventListener: CreateEventListener(dotNetObjectReference)
    };

    charts[dotNetObjectReference._id] = chart;
    window.addEventListener('resize', chart.eventListener);
}

function CreateEventListener(dotNetObjectReference) {
    return function () {
        dotNetObjectReference.invokeMethodAsync('StateHasChanged');
    };
}

function CreateSvgElement() {
    const svgElement = document.createElementNS(svgNamespace, "svg");
    svgElement.setAttribute("class", "chart-main");
    svgElement.setAttribute("xmlns", svgNamespace);
    svgElement.setAttribute("viewBox", "0 0 1 1");
    svgElement.setAttribute("width", "1");
    svgElement.setAttribute("height", "1");
    svgElement.setAttribute("style", "visibility: hidden;");
    document.body.appendChild(svgElement);

    const groupElement = document.createElementNS(svgNamespace, "g");
    svgElement.appendChild(groupElement);

    return { svgElement, groupElement };
}

function unregister(dotNetObjectReference) {
    const chart = charts[dotNetObjectReference._id];

    if (chart) {
        chart.svgElement.removeChild(chart.groupElement);
        document.body.removeChild(chart.svgElement);
        window.removeEventListener('resize', chart.eventListener);

        delete charts[dotNetObjectReference._id];
    }
}

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

function getBoundingBox(dotNetObjectReference, text, cssClass) {
    const groupElement = charts[dotNetObjectReference._id].groupElement;
    const textElement = document.createElementNS(svgNamespace, "text");
    textElement.setAttribute("class", cssClass);
    textElement.textContent = text;
    groupElement.appendChild(textElement);

    const bbox = groupElement.getBBox();

    groupElement.removeChild(textElement);

    return {
        x: bbox.x,
        y: bbox.y,
        width: bbox.width,
        height: bbox.height
    };
}

export { register, unregister, getAvailableWidth, getBoundingBox };
