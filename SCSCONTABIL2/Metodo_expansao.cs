using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xceed.Wpf.Toolkit;

namespace Xceed.Wpf.Toolkit
{
    public static class Methods
    {
        public static string semFormato(this MaskedTextBox _mask)
        {
            //metodo de extensão que retira a formatação do conteudo para adicionar no BD
            
            String retString = _mask.Text.Replace(".", "").Replace("/", "").Replace(",", "").Replace("-", "");
            
            return retString;
        }
    }
}


