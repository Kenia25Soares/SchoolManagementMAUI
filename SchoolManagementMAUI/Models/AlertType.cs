namespace SchoolManagementMAUI.Models
{
    public enum AlertType
    {
        // Alertas principais do sistema
        GradePosted,
        AddedToClass,
        StatusChanged,
        RemovedFromClass,
        ClassClosed,
        ExcludedByAbsences,
        GeneralNotification,
        
        // Status de aprovação/reprovação
        Approved,
        Failed
    }
}
