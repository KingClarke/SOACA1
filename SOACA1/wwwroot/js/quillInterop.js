window.quillInterop = {
  editors: {},
  create: function (elementId, dotNetRef) {
    const quill = new Quill('#' + elementId, { theme: 'snow' });
    quill.on('text-change', () => {
      dotNetRef.invokeMethodAsync('OnContentChanged',
        quill.root.innerHTML, quill.getText());
    });
    this.editors[elementId] = quill;
    return quill.root.innerHTML;
  },
  getHtml: id => window.quillInterop.editors[id]?.root.innerHTML ?? '',
  setHtml: (id, html) => {
    const q = window.quillInterop.editors[id];
    if (q) { q.root.innerHTML = html; }
  },
  dispose: id => { delete window.quillInterop.editors[id]; }
};