
const svgNamespace = "http://www.w3.org/2000/svg";
let svgElement;

function register() {
    svgElement = document.createElementNS(svgNamespace, "svg");
    svgElement.setAttribute("class", "chart-main");
    svgElement.setAttribute("xmlns", svgNamespace);
    svgElement.setAttribute("viewBox", "0 0 1 1");
    svgElement.setAttribute("width", "1");
    svgElement.setAttribute("height", "1");
    svgElement.setAttribute("style", "visibility: hidden;");
    document.body.appendChild(svgElement);
}

function unregister() {
    document.body.removeChild(svgElement);
}

function getTextSize(text, cssClass) {
    const textElement = document.createElementNS(svgNamespace, "text");
    textElement.setAttribute("class", cssClass);
    textElement.textContent = text;
    svgElement.appendChild(textElement);

    const bbox = textElement.getBBox();

    svgElement.removeChild(textElement);

    return {
        width: bbox.width,
        height: bbox.height
    };
}

export { register, unregister, getTextSize };
