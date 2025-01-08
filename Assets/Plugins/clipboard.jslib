mergeInto(LibraryManager.library, {
    CopyToClipboard: function (textPtr) {
        var text = UTF8ToString(textPtr);
        var input = document.createElement('input');
        input.value = text;
        document.body.appendChild(input);
        input.select();
        document.execCommand('copy');
        document.body.removeChild(input);
        console.log('Copied to clipboard: ' + text);
    }
});
