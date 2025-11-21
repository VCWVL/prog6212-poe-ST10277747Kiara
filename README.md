Contract Monthly Claim System (CMCS)

Student: Kiara Israel
Student Number: ST10277747

About the Application:
The Contract Monthly Claim System (CMCS) is a web-based application designed to streamline the process of lecturers submitting monthly claims for hours worked. It allows lecturers to submit claims with relevant details and supporting documents, while Programme Coordinators and Academic Managers can review, approve, or reject these claims. The system tracks claim status, maintains a history of approvals, and ensures proper role-based access, all within a responsive and user-friendly interface.

The Features Include:
Claim Submission: Lecturers can submit claims with hours worked, hourly rate, and notes. Claim amounts are calculated automatically. Each claim is linked to a lecturer and includes supporting documents uploaded from File Explorer.

Claim Approval: Programme Coordinators and Academic Managers can approve or reject claims. Claim history is updated with approver details and dates.

Status Tracking: Claims display their current status: Pending, Approved, or Rejected.

UI Design: Responsive, clean interface using Bootstrap-styled forms and buttons.

HR View and Invoice Generation: Once the Academic Manager approves a claim, the claim becomes visible in the HR view. The HR department can access all verified claims to ensure accuracy before processing. Each lecturer’s approved claim is used to generate a PDF invoice that includes lecturer details, hours worked, hourly rate, total claim amount, and approval history. This process ensures accurate payroll and provides a complete record for documentation purposes.

What I Fixed and the Improvements:
Following the lecturers’ feedback, the documentation part of the CMCS project was reviewed and updated. The main improvements are:

Documentation: Assumptions and Constraints:
Previous Issue: Redundancy present in entities; assumptions and constraints not clearly stated.
Fix Implemented: I removed redundant entities and normalized the structure using User, Role, and Approval. Assumptions and constraints are now clearly stated, e.g., lecturers can submit only one claim per month per contract, supporting documents must be in PDF or DOCX format, and Programme Coordinators and Academic Managers have defined approval roles. Entity relationships are clarified, avoiding duplicated foreign keys.
Highlight: All assumptions and constraints are documented and highlighted in the updated documentation for proof of correction.

UML Class Diagram for Databases: Accuracy and Completeness:
Previous Issue: Dependencies not fully specified; relationships unclear.
Fix Implemented: UML diagram updated to include all entities (User, Claim, Approval, SupportingDocument) with clear relationships and cardinality. Redundant links removed; database design fully normalized. Attributes, primary keys, and foreign keys clearly defined.
Highlight: Dependencies and class relationships are now fully accurate and complete.

Project Plan: Realism and Achievability:
Previous Issue: Timelines were realistic but not clearly linked to prototype deliverables.
Fix Implemented: Adjusted milestones to align with actual development phases, including submission, approval, HR verification, and review. Added contingency time for testing and bug fixing. Each milestone now corresponds to a prototype or deliverable.
Highlight: Project plan demonstrates achievable goals aligned with prototype functionality.

Proof of Fixes:
Documentation now highlights assumptions and constraints. UML diagram updated with dependencies and normalized relationships. Project plan refined with realistic milestones and deliverables. HR view and invoice generation have been integrated into the workflow to complete the claim approval and payroll process.

This is How to Run My Web App:
Make sure you have Visual Studio installed. Ensure .NET 6/7/8 SDK is installed (I used .NET 8). Clone or download the repository. Open the solution in Visual Studio 2022. Build dependencies and run the project.

YouTube Links:

To view the code running, access the demonstration at https://youtu.be/0R-8H-rC1bc

To see the web application running, visit https://youtu.be/9lAAm6n5HfY
