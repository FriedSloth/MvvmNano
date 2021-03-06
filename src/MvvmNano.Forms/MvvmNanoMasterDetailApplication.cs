﻿using System.Collections.ObjectModel;

namespace MvvmNano.Forms
{
    /// <summary>
    /// The entry point of you XF application, if you want to use master detail (fly out menu) navigation.
    /// </summary>
    public abstract class MvvmNanoMasterDetailApplication : MvvmNanoApplication 
    {
        /// <summary>
        /// A collection of MvvmNanoMasterDetailData that represents available detail pages.
        /// </summary>
        public ObservableCollection<MvvmNanoMasterDetailData> MasterDetails { get; set; } = new ObservableCollection<MvvmNanoMasterDetailData>();

        /// <summary>
        /// The <see cref="MvvmNanoMasterDetailPage"/> if one is set. 
        /// </summary>
        public MvvmNanoMasterDetailPage MasterPage => MainPage as MvvmNanoMasterDetailPage;

        /// <summary>
        /// Sets up the main page as a <see cref="MvvmNanoMasterDetailPage{TViewModel}"/> for the given view model type.
        /// </summary> 
        public new void SetUpMainPage<TViewModel>() where TViewModel : MvvmNanoViewModel
        { 
            MainPage = GetMasterDetailPageFor<TViewModel>();
            if (MasterDetails.Count > 0)
                MasterPage.SetDetail(MasterDetails[0]); 
        }

        /// <summary>
        /// Add a site to the <see cref="MasterDetails"/>.
        /// </summary>
        /// <param name="data"><see cref="MvvmNanoMasterDetailData"/> with information for the detail site.</param>
        public void AddPageToDetailPages<TViewModel>(MvvmNanoMasterDetailData data) where TViewModel : MvvmNanoViewModelBase
        {
            data.ViewModelType = typeof(TViewModel);
            MasterDetails.Add(data);

            //Check if a MasterPage is already set up and set the new Detail page as Detail if there is no Detail set yet.
            if (MasterPage != null)
            {
                var presenter = MvvmNanoIoC.Resolve<IPresenter>() as MvvmNanoFormsPresenter;
                if (presenter != null && presenter.CurrentPage == null)
                    MasterPage.SetDetail(data);
            }
        } 

        /// <summary>
        /// Creates a <see cref="MvvmNanoMasterDetailPage{TViewModel}"/> for the given view model.
        /// </summary> 
        public MvvmNanoMasterDetailPage<TViewModel> GetMasterDetailPageFor<TViewModel>() where TViewModel : MvvmNanoViewModel
        {
            TViewModel viewModel = MvvmNanoIoC.Resolve<TViewModel>();
            viewModel.Initialize();
            MvvmNanoMasterDetailPage<TViewModel> viewFor = MvvmNanoIoC.Resolve<IPresenter>().CreateViewFor<TViewModel>() as MvvmNanoMasterDetailPage<TViewModel>;
            if (viewFor == null)
                throw new MvvmNanoException("Could not create a MvvmNanoMasterDetailPage for View Model of type " + typeof(TViewModel) + ".");
            viewFor.SetViewModel(viewModel);
            return viewFor;
        }
    }
}
