﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using marr.BusinessRule;
using System.Text;
using marr.DataLayer;

namespace UI.wxws.setup
{
    public partial class wx_violation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
              /*  using (brWebnote br = new brWebnote())
                {

                    xt_webnote ob = br.Retrieve("v001");
                    if (ob != null)
                    {
                        this.content.InnerHtml = ob.sys_note;

                    }
                    else
                    {
                        this.content.InnerHtml = "暂无违规公告";
                    }
                }*/
            }
        }
    }
}