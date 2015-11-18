using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xceed.Wpf.Toolkit;

namespace extensao
{
    public static class Methods
    {
        public static string semFormato(this MaskedTextBox _mask)
        {
            //metodo de extensão que retira a formatação do conteudo para adicionar no BD
            _mask.ClipboardMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            String retString = _mask.Text;
            _mask.ClipboardMaskFormat = MaskFormat.IncludePromptAndLiterals;
            return retString;
        }
    }
}


