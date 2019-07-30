using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbobsCOM;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using Application = SAPbouiCOM.Framework.Application;

namespace CashFlowUpdate.SystemForms
{
    [FormAttribute("809", "SystemForms/AccountBalance.b1f")]
    class AccountBalance : SystemFormBase
    {
        public AccountBalance()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("Item_0").Specific));
            this.Button0.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button0_PressedAfter);
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
        }

        private SAPbouiCOM.Button Button0;

        private void OnCustomInitialize()
        {

        }

        private void Button0_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            Form accountBalance = SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm;
            Matrix matrix = (Matrix)accountBalance.Items.Item("3").Specific; 
            for (int i = 1; i <= matrix.RowCount; i++)
            {
                var transId = ((EditText)matrix.GetCellSpecific(4, i)).Value;
                if (string.IsNullOrWhiteSpace(transId))
                {
                    continue;
                }

                JournalEntries journalEntry = (SAPbobsCOM.JournalEntries)UIConnection.xCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries);
                journalEntry.GetByKey(int.Parse(transId));

                for (int j = 0; j < journalEntry.Lines.Count; j++)
                {
                    journalEntry.Lines.SetCurrentLine(j);
                    if (journalEntry.Lines.AccountCode == "1110/001/007")
                    {
                        journalEntry.Lines.PrimaryFormItems.CashFlowLineItemID = 44;
                    }
                    var result = journalEntry.Update();
                    if (result != 0)
                    {
                        Application.SBO_Application.SetStatusBarMessage(UIConnection.xCompany.GetLastErrorDescription(),
                            BoMessageTime.bmt_Short, true);
                    }
                }

            }
        }
    }
}
