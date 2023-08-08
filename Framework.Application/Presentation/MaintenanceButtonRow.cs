using Framework.Core.Resources;
using System;
using System.Collections.Generic;

namespace Framework.Application.Presentation
{
    public class MaintenanceButtonRow
    {
        #region Preset enum

        public enum Preset
        {
            CreateCancel,
            SaveDeleteCancel,
            SaveSendDeleteCancel,
            ResendCancel,
            Submit,
            Cancel,
            Close,
            Save,
            SubmitClose,
            ViewClose,
            AcceptRejectViewClose,
            View,
            Archive
        }

        #endregion

        #region Fields

        private List<GenericButton> _items;

        #endregion

        #region Constructors

        public MaintenanceButtonRow(Preset preset)
        {
            switch (preset)
            {
                case Preset.CreateCancel:
                    Items.Add(GetCreateButton());
                    Items.Add(GetCancelButton());
                    break;

                case Preset.SaveDeleteCancel:
                    Items.Add(GetSaveButton());
                    Items.Add(GetDeleteButton());
                    Items.Add(GetCancelButton());
                    break;

                case Preset.SaveSendDeleteCancel:
                    Items.Add(GetSaveButton());
                    Items.Add(GetSendButton());
                    Items.Add(GetDeleteButton());
                    Items.Add(GetCancelButton());
                    break;

                case Preset.ResendCancel:
                    Items.Add(GetResendButton());
                    Items.Add(GetCancelButton());
                    break;

                case Preset.Submit:
                    Items.Add(GetSubmitButton());
                    break;

                case Preset.Cancel:
                    Items.Add(GetCancelButton());
                    break;

                case Preset.Close:
                    Items.Add(GetCloseButton());
                    break;

                case Preset.Save:
                    Items.Add(GetSaveButton());
                    break;

                case Preset.SubmitClose:
                    Items.Add(GetSubmitButton());
                    Items.Add(GetCloseButton());
                    break;

                case Preset.ViewClose:
                    Items.Add(GetViewButton());
                    Items.Add(GetCloseButton());
                    break;

                case Preset.AcceptRejectViewClose:
                    Items.Add(GetAcceptButton());
                    Items.Add(GetRejectButton());
                    Items.Add(GetViewButton());
                    Items.Add(GetCloseButton());
                    break;

                case Preset.View:
                    Items.Add(GetViewButton());
                    break;

                case Preset.Archive:
                    Items.Add(GetArchiveButton());
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion

        #region Properties

        public ICollection<GenericButton> Items => _items ?? (_items = new List<GenericButton>());

        #endregion

        #region Private Methods

        private static GenericButton GetCreateButton()
        {
            return GetCustomButton("createButton", GeneralResource.General_Create);
        }

        private static GenericButton GetSaveButton()
        {
            return GetCustomButton("saveButton", GeneralResource.General_Save);
        }

        private static GenericButton GetSendButton()
        {
            return GetCustomButton("sendButton", GeneralResource.General_Send, PresentationConstant.ColorUtility.Success);
        }

        private static GenericButton GetResendButton()
        {
            return GetCustomButton("resendButton", GeneralResource.General_Resend, PresentationConstant.ColorUtility.Success);
        }

        private static GenericButton GetSubmitButton()
        {
            return GetCustomButton("submitButton", GeneralResource.General_Submit);
        }

        private static GenericButton GetDeleteButton()
        {
            return GetCustomButton("deleteButton", GeneralResource.General_Delete, PresentationConstant.ColorUtility.Danger);
        }

        private static GenericButton GetArchiveButton()
        {
            return GetCustomButton("archiveButton", GeneralResource.General_Archive);
        }

        private static GenericButton GetCancelButton()
        {
            return GetCustomButton("cancelButton", GeneralResource.General_Cancel, PresentationConstant.ColorUtility.Default);
        }

        private static GenericButton GetCloseButton()
        {
            return GetCustomButton("closeButton", GeneralResource.General_Close, PresentationConstant.ColorUtility.Default);
        }

        private static GenericButton GetAcceptButton()
        {
            return GetCustomButton("acceptButton", GeneralResource.General_Accept, PresentationConstant.ColorUtility.Success);
        }

        private static GenericButton GetRejectButton()
        {
            return GetCustomButton("rejectButton", GeneralResource.General_Reject, PresentationConstant.ColorUtility.Danger);
        }

        private static GenericButton GetViewButton()
        {
            return GetCustomButton("viewButton", GeneralResource.General_View);
        }

        private static GenericButton GetCustomButton(string id,
            string label,
            string colorUtility = PresentationConstant.ColorUtility.Primary)
        {
            var cssClass = "btn btn-default";
            switch (colorUtility)
            {
                case PresentationConstant.ColorUtility.Primary:
                    cssClass = "btn btn-primary";
                    break;
                case PresentationConstant.ColorUtility.Info:
                    cssClass = "btn btn-info";
                    break;
                case PresentationConstant.ColorUtility.Success:
                    cssClass = "btn btn-success";
                    break;
                case PresentationConstant.ColorUtility.Warning:
                    cssClass = "btn btn-warning";
                    break;
                case PresentationConstant.ColorUtility.Danger:
                    cssClass = "btn btn-danger";
                    break;
            }

            return new GenericButton
            {
                Id = id,
                Type = PresentationConstant.ButtonType.Button,
                Label = label,
                CssClass = cssClass
            };
        }

        #endregion
    }
}