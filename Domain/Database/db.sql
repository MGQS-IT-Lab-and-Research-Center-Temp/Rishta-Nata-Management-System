-- ============================================================
-- Marriage Application Dummy Data
-- MySQL
-- ============================================================

-- START TRANSACTION;

-- ============================================================
-- 1. MarriageApplications
-- ============================================================

INSERT INTO MarriageApplications
(
    Id,
    Status,
    UserId,
    SerialNumber
)
VALUES
(
    '10000000-0000-4000-8000-000000000001',
    1,
    '20000000-0000-4000-8000-000000000001',
    'NIKAH/2026/0001'
),
(
    '10000000-0000-4000-8000-000000000002',
    1,
    '20000000-0000-4000-8000-000000000002',
    'NIKAH/2026/0002'
),
(
    '10000000-0000-4000-8000-000000000003',
    1,
    '20000000-0000-4000-8000-000000000003',
    'NIKAH/2026/0003'
),
(
    '10000000-0000-4000-8000-000000000004',
    1,
    '20000000-0000-4000-8000-000000000004',
    'NIKAH/2026/0004'
),
(
    '10000000-0000-4000-8000-000000000005',
    1,
    '20000000-0000-4000-8000-000000000005',
    'NIKAH/2026/0005'
),
(
    '10000000-0000-4000-8000-000000000006',
    1,
    '20000000-0000-4000-8000-000000000006',
    'NIKAH/2026/0006'
),
(
    '10000000-0000-4000-8000-000000000007',
    1,
    '20000000-0000-4000-8000-000000000007',
    'NIKAH/2026/0007'
),
(
    '10000000-0000-4000-8000-000000000008',
    1,
    '20000000-0000-4000-8000-000000000008',
    'NIKAH/2026/0008'
),
(
    '10000000-0000-4000-8000-000000000009',
    1,
    '20000000-0000-4000-8000-000000000009',
    'NIKAH/2026/0009'
),
(
    '10000000-0000-4000-8000-000000000010',
    1,
    '20000000-0000-4000-8000-000000000010',
    'NIKAH/2026/0010'
);


-- ============================================================
-- 2. MarriageApplicationForms
-- ============================================================

INSERT INTO MarriageApplicationForms
(
    Id,
    MarriageApplicationId,
    ReferenceNumber,
    ProposedNikahDate,
    Venue,

    BrideMembershipNo,
    BrideName,
    BrideDateOfBirth,
    BrideResidentOf,
    BrideGenotype,
    BrideBloodGroup,
    BrideMaritalStatus,
    BrideProposedDowerAmount,
    BrideDowerAmountReceivedInCash,
    BrideSignatureTel,

    BridegroomMembershipNo,
    BridegroomName,
    BridegroomDateOfBirth,
    BridegroomResidentOf,
    BridegroomGenotype,
    BridegroomBloodGroup,
    BridegroomDowerAmountPaidInCash,
    BridegroomDowerAmountToBePaid,
    IsFirstNikah,
    IsSecondThirdOrFourthNikah,
    FormerWifeIsDead,
    HasDivorcedFormerWife,
    FormerWifeIsPresent,
    FormerWifeObtainedKhula,
    BridegroomSignatureTel,

    BrideFatherName,
    BridegroomFatherName,

    GuardianName,
    GuardianRelationToBride,
    GuardianAddress,
    GuardianTel,
    GuardianSignatureDate,

    RepresentativeName,
    RepresentativeAddress,
    RepresentativeActingFor,
    RepresentativeSignatureDate,

    WitnessOneName,
    WitnessOneAddress,
    WitnessOneTel,
    WitnessOneSignatureDate,

    WitnessTwoName,
    WitnessTwoAddress,
    WitnessTwoTel,
    WitnessTwoSignatureDate,

    OfficiatingImamName,
    OfficiatingImamAddressJamaat,
    OfficiatingImamSignatureDate,

    JamaatPresidentName,
    JamaatPresidentSignatureDate,

    NationalRishtanataSecretaryName,
    NationalRishtanataSecretarySignatureDate,
    ApprovedDateOfNikah,
    NationalAmirOrMissionarySignatureDate
)
VALUES

-- ============================================================
-- Application 1
-- ============================================================

(
    '30000000-0000-4000-8000-000000000001',
    '10000000-0000-4000-8000-000000000001',
    'AMJN/NF/0001',
    '2026-09-05 00:00:00',
    'Lagos Central Jama''at',

    '12458',
    'Aisha Mariam Bello',
    '1998-03-14 00:00:00',
    'Surulere, Lagos',
    'AA',
    'O+',
    'Unmarried',
    750000,
    250000,
    '08031234567',

    '28741',
    'Abdul Kareem Yusuf',
    '1995-07-22 00:00:00',
    'Yaba, Lagos',
    'AS',
    'O+',
    250000,
    500000,
    TRUE,
    FALSE,
    FALSE,
    FALSE,
    FALSE,
    FALSE,
    '08069876543',

    'D/o Ibrahim Bello',
    'S/o Abdul Rahman Yusuf',

    'Ibrahim Bello',
    'Father',
    'Surulere, Lagos',
    '08034561278',
    '2026-08-20',

    '',
    '',
    '',
    '',

    'Musa Abdullahi',
    'Ikeja, Lagos',
    '08045678912',
    '2026-08-20',

    'Sadiq Ahmed',
    'Agege, Lagos',
    '08123456789',
    '2026-08-20',

    'Imam Abdul Hafiz',
    'Lagos Central Jama''at',
    '2026-08-21',

    'Dr. Hamid Salako',
    '2026-08-21',

    'Mubarak Lawal',
    '2026-08-22',
    '2026-09-05 00:00:00',
    '2026-08-23'
),

-- ============================================================
-- Application 2
-- ============================================================

(
    '30000000-0000-4000-8000-000000000002',
    '10000000-0000-4000-8000-000000000002',
    'AMJN/NF/0002',
    '2026-09-12 00:00:00',
    'Ikeja Jama''at',

    '3512',
    'Fatima Zahra Sule',
    '2000-11-09 00:00:00',
    'Ikeja, Lagos',
    'AA',
    'A+',
    'Unmarried',
    500000,
    100000,
    '08134567890',

    '19873',
    'Abdul Malik Hassan',
    '1997-02-18 00:00:00',
    'Gbagada, Lagos',
    'AA',
    'B+',
    100000,
    400000,
    TRUE,
    FALSE,
    FALSE,
    FALSE,
    FALSE,
    FALSE,
    '07034567891',

    'D/o Suleiman Sule',
    'S/o Hassan Abdul',

    'Suleiman Sule',
    'Father',
    'Ikeja, Lagos',
    '08056781234',
    '2026-08-25',

    '',
    '',
    '',
    '',

    'Yahya Ibrahim',
    'Maryland, Lagos',
    '08156782345',
    '2026-08-25',

    'Abbas Mohammed',
    'Ojota, Lagos',
    '08067893456',
    '2026-08-25',

    'Imam Muhammad Bashir',
    'Ikeja Jama''at',
    '2026-08-26',

    'Musa Garba',
    '2026-08-26',

    'Mubarak Lawal',
    '2026-08-27',
    '2026-09-12 00:00:00',
    '2026-08-28'
),

-- ============================================================
-- Application 3
-- ============================================================

(
    '30000000-0000-4000-8000-000000000003',
    '10000000-0000-4000-8000-000000000003',
    'AMJN/NF/0003',
    '2026-09-19 00:00:00',
    'Abuja Central Jama''at',

    '44219',
    'Khadijah Amina Musa',
    '1996-06-27 00:00:00',
    'Wuse, Abuja',
    'AS',
    'B+',
    'Unmarried',
    900000,
    300000,
    '08078912345',

    '17326',
    'Sadiq Umar',
    '1993-12-03 00:00:00',
    'Gwarinpa, Abuja',
    'AA',
    'B+',
    300000,
    600000,
    TRUE,
    FALSE,
    FALSE,
    FALSE,
    FALSE,
    FALSE,
    '08178945612',

    'D/o Musa Ibrahim',
    'S/o Umar Bello',

    'Musa Ibrahim',
    'Father',
    'Wuse, Abuja',
    '08089012345',
    '2026-08-29',

    '',
    '',
    '',
    '',

    'Abubakar Danjuma',
    'Maitama, Abuja',
    '08090123456',
    '2026-08-29',

    'Haruna Yakubu',
    'Garki, Abuja',
    '08101234567',
    '2026-08-29',

    'Imam Abdul Qadir',
    'Abuja Central Jama''at',
    '2026-08-30',

    'Dr. Salihu Ibrahim',
    '2026-08-30',

    'Mubarak Lawal',
    '2026-08-31',
    '2026-09-19 00:00:00',
    '2026-09-01'
),

-- ============================================================
-- Application 4
-- ============================================================

(
    '30000000-0000-4000-8000-000000000004',
    '10000000-0000-4000-8000-000000000004',
    'AMJN/NF/0004',
    '2026-10-03 00:00:00',
    'Kano Jama''at',

    '21904',
    'Maryam Halima Abdullahi',
    '1999-01-15 00:00:00',
    'Nassarawa, Kano',
    'AA',
    'O+',
    'Unmarried',
    600000,
    200000,
    '08012349876',

    '30761',
    'Ibrahim Suleiman',
    '1994-08-11 00:00:00',
    'Fagge, Kano',
    'AA',
    'O+',
    200000,
    400000,
    TRUE,
    FALSE,
    FALSE,
    FALSE,
    FALSE,
    FALSE,
    '08112349876',

    'D/o Abdullahi Garba',
    'S/o Suleiman Ibrahim',

    'Abdullahi Garba',
    'Father',
    'Nassarawa, Kano',
    '08023456781',
    '2026-09-05',

    '',
    '',
    '',
    '',

    'Usman Bello',
    'Fagge, Kano',
    '08034567812',
    '2026-09-05',

    'Abdulaziz Musa',
    'Gwale, Kano',
    '08145678923',
    '2026-09-05',

    'Imam Bashir Ahmad',
    'Kano Jama''at',
    '2026-09-06',

    'Abdulmajid Usman',
    '2026-09-06',

    'Mubarak Lawal',
    '2026-09-07',
    '2026-10-03 00:00:00',
    '2026-09-08'
),

-- ============================================================
-- Application 5
-- ============================================================

(
    '30000000-0000-4000-8000-000000000005',
    '10000000-0000-4000-8000-000000000005',
    'AMJN/NF/0005',
    '2026-10-10 00:00:00',
    'Ibadan Jama''at',

    '8765',
    'Safiya Rahmat Adeyemi',
    '1997-04-19 00:00:00',
    'Bodija, Ibadan',
    'AA',
    'A+',
    'Divorced (waited)',
    450000,
    150000,
    '08123450987',

    '39108',
    'Faruq Olatunji',
    '1991-09-30 00:00:00',
    'Jericho, Ibadan',
    'AS',
    'A+',
    150000,
    300000,
    FALSE,
    TRUE,
    FALSE,
    TRUE,
    FALSE,
    FALSE,
    '08098765012',

    'D/o Adeyemi Johnson',
    'S/o Olatunji Kareem',

    'Adeyemi Johnson',
    'Father',
    'Bodija, Ibadan',
    '08045670123',
    '2026-09-12',

    'Abdul Rahman Adeyemi',
    'Bodija, Ibadan',
    'Guardian',
    '2026-09-12',

    'Rashid Adebayo',
    'Mokola, Ibadan',
    '08156781234',
    '2026-09-12',

    'Khalid Yusuf',
    'Apata, Ibadan',
    '08067892345',
    '2026-09-12',

    'Imam Suleiman Abdullahi',
    'Ibadan Jama''at',
    '2026-09-13',

    'Abdul Wasiu Lawal',
    '2026-09-13',

    'Mubarak Lawal',
    '2026-09-14',
    '2026-10-10 00:00:00',
    '2026-09-15'
),

-- ============================================================
-- Application 6
-- ============================================================

(
    '30000000-0000-4000-8000-000000000006',
    '10000000-0000-4000-8000-000000000006',
    'AMJN/NF/0006',
    '2026-10-17 00:00:00',
    'Port Harcourt Jama''at',

    '46821',
    'Hauwa Nasreen Danladi',
    '2001-02-25 00:00:00',
    'Rumuola, Port Harcourt',
    'AA',
    'B+',
    'Unmarried',
    800000,
    300000,
    '08076543210',

    '14267',
    'Yusuf Chukwu',
    '1996-10-07 00:00:00',
    'D-Line, Port Harcourt',
    'AA',
    'B+',
    300000,
    500000,
    TRUE,
    FALSE,
    FALSE,
    FALSE,
    FALSE,
    FALSE,
    '08165432109',

    'D/o Danladi Musa',
    'S/o Chukwu Emmanuel',

    'Danladi Musa',
    'Father',
    'Rumuola, Port Harcourt',
    '08087654321',
    '2026-09-19',

    '',
    '',
    '',
    '',

    'Bilal Okoro',
    'GRA, Port Harcourt',
    '08076549812',
    '2026-09-19',

    'Hamza Ibrahim',
    'Eliozu, Port Harcourt',
    '08165430987',
    '2026-09-19',

    'Imam Abdul Karim',
    'Port Harcourt Jama''at',
    '2026-09-20',

    'Ibrahim Nwosu',
    '2026-09-20',

    'Mubarak Lawal',
    '2026-09-21',
    '2026-10-17 00:00:00',
    '2026-09-22'
),

-- ============================================================
-- Application 7
-- ============================================================

(
    '30000000-0000-4000-8000-000000000007',
    '10000000-0000-4000-8000-000000000007',
    'AMJN/NF/0007',
    '2026-10-24 00:00:00',
    'Kaduna Jama''at',

    '16243',
    'Zainab Rukayya Ibrahim',
    '1995-05-12 00:00:00',
    'Barnawa, Kaduna',
    'AS',
    'O+',
    'Widowed (waited)',
    700000,
    200000,
    '08034561290',

    '28406',
    'Abdul Wahid Sani',
    '1989-11-21 00:00:00',
    'Kawo, Kaduna',
    'AA',
    'O+',
    200000,
    500000,
    FALSE,
    TRUE,
    TRUE,
    FALSE,
    FALSE,
    FALSE,
    '08123456701',

    'D/o Ibrahim Yusuf',
    'S/o Sani Abdul',

    'Ibrahim Yusuf',
    'Brother',
    'Barnawa, Kaduna',
    '08045672310',
    '2026-09-26',

    '',
    '',
    '',
    '',

    'Abdul Samad Musa',
    'Kawo, Kaduna',
    '08056783421',
    '2026-09-26',

    'Suleiman Garba',
    'Malali, Kaduna',
    '08167894532',
    '2026-09-26',

    'Imam Abdul Latif',
    'Kaduna Jama''at',
    '2026-09-27',

    'Nasir Bello',
    '2026-09-27',

    'Mubarak Lawal',
    '2026-09-28',
    '2026-10-24 00:00:00',
    '2026-09-29'
),

-- ============================================================
-- Application 8
-- ============================================================

(
    '30000000-0000-4000-8000-000000000008',
    '10000000-0000-4000-8000-000000000008',
    'AMJN/NF/0008',
    '2026-11-07 00:00:00',
    'Benin Jama''at',

    '32567',
    'Sumayyah Esther Eze',
    '1998-08-30 00:00:00',
    'GRA, Benin City',
    'AA',
    'A+',
    'Unmarried',
    550000,
    150000,
    '08123451234',

    '41029',
    'Mubarak Eze',
    '1995-03-16 00:00:00',
    'Ugbowo, Benin City',
    'AA',
    'A+',
    150000,
    400000,
    TRUE,
    FALSE,
    FALSE,
    FALSE,
    FALSE,
    FALSE,
    '08087651234',

    'D/o Peter Eze',
    'S/o Emmanuel Eze',

    'Peter Eze',
    'Father',
    'GRA, Benin City',
    '08034562345',
    '2026-10-10',

    '',
    '',
    '',
    '',

    'Ibrahim Yusuf',
    'Ugbowo, Benin City',
    '08145673456',
    '2026-10-10',

    'Mustapha Bello',
    'Ikpoba Hill, Benin City',
    '08056784567',
    '2026-10-10',

    'Imam Abdul Mateen',
    'Benin Jama''at',
    '2026-10-11',

    'Yusuf Osagie',
    '2026-10-11',

    'Mubarak Lawal',
    '2026-10-12',
    '2026-11-07 00:00:00',
    '2026-10-13'
),

-- ============================================================
-- Application 9
-- ============================================================

(
    '30000000-0000-4000-8000-000000000009',
    '10000000-0000-4000-8000-000000000009',
    'AMJN/NF/0009',
    '2026-11-14 00:00:00',
    'Lagos Mainland Jama''at',

    '5298',
    'Ruqayyah Binta Ahmed',
    '2000-12-17 00:00:00',
    'Yaba, Lagos',
    'AA',
    'O+',
    'Unmarried',
    650000,
    200000,
    '08012345678',

    '24681',
    'Abdul Basit Kareem',
    '1992-06-08 00:00:00',
    'Surulere, Lagos',
    'AA',
    'O+',
    200000,
    450000,
    TRUE,
    FALSE,
    FALSE,
    FALSE,
    FALSE,
    FALSE,
    '08198765432',

    'D/o Ahmed Bello',
    'S/o Kareem Musa',

    'Ahmed Bello',
    'Father',
    'Yaba, Lagos',
    '08023456789',
    '2026-10-17',

    '',
    '',
    '',
    '',

    'Abdul Hadi Musa',
    'Ebute Metta, Lagos',
    '08034567890',
    '2026-10-17',

    'Sulaiman Ade',
    'Mushin, Lagos',
    '08145678901',
    '2026-10-17',

    'Imam Abdul Wahab',
    'Lagos Mainland Jama''at',
    '2026-10-18',

    'Hamza Balogun',
    '2026-10-18',

    'Mubarak Lawal',
    '2026-10-19',
    '2026-11-14 00:00:00',
    '2026-10-20'
),

-- ============================================================
-- Application 10
-- ============================================================

(
    '30000000-0000-4000-8000-000000000010',
    '10000000-0000-4000-8000-000000000010',
    'AMJN/NF/0010',
    '2026-11-21 00:00:00',
    'Ilorin Jama''at',

    '18734',
    'Nafisat Modupe Lawal',
    '1999-07-04 00:00:00',
    'Tanke, Ilorin',
    'AS',
    'B+',
    'Unmarried',
    500000,
    100000,
    '08065439876',

    '35642',
    'Abdullah Abdulraheem',
    '1994-01-26 00:00:00',
    'GRA, Ilorin',
    'AA',
    'B+',
    100000,
    400000,
    TRUE,
    FALSE,
    FALSE,
    FALSE,
    FALSE,
    FALSE,
    '08176543210',

    'D/o Modupe Lawal',
    'S/o Abdulraheem Yusuf',

    'Modupe Lawal',
    'Father',
    'Tanke, Ilorin',
    '08076540123',
    '2026-10-24',

    '',
    '',
    '',
    '',

    'Abdullahi Sanni',
    'Fate, Ilorin',
    '08187651234',
    '2026-10-24',

    'Murtala Ibrahim',
    'Adewole, Ilorin',
    '08098762345',
    '2026-10-24',

    'Imam Abdul Fattah',
    'Ilorin Jama''at',
    '2026-10-25',

    'Abdul Ganiyu Yusuf',
    '2026-10-25',

    'Mubarak Lawal',
    '2026-10-26',
    '2026-11-21 00:00:00',
    '2026-10-27'
);


-- ============================================================
-- 3. Certificates
-- ============================================================

INSERT INTO Certificates
(
    Id,
    MarriageApplicationId,
    IssueDate,
    CertificateFilePath,
    IssuedByUserId
)
VALUES
(
    '40000000-0000-4000-8000-000000000001',
    '10000000-0000-4000-8000-000000000001',
    '2026-09-06 00:00:00',
    '/certificates/NIKAH-2026-0001.pdf',
    '20000000-0000-4000-8000-000000000001'
),
(
    '40000000-0000-4000-8000-000000000002',
    '10000000-0000-4000-8000-000000000002',
    '2026-09-13 00:00:00',
    '/certificates/NIKAH-2026-0002.pdf',
    '20000000-0000-4000-8000-000000000002'
),
(
    '40000000-0000-4000-8000-000000000003',
    '10000000-0000-4000-8000-000000000003',
    '2026-09-20 00:00:00',
    '/certificates/NIKAH-2026-0003.pdf',
    '20000000-0000-4000-8000-000000000003'
),
(
    '40000000-0000-4000-8000-000000000004',
    '10000000-0000-4000-8000-000000000004',
    '2026-10-04 00:00:00',
    '/certificates/NIKAH-2026-0004.pdf',
    '20000000-0000-4000-8000-000000000004'
),
(
    '40000000-0000-4000-8000-000000000005',
    '10000000-0000-4000-8000-000000000005',
    '2026-10-11 00:00:00',
    '/certificates/NIKAH-2026-0005.pdf',
    '20000000-0000-4000-8000-000000000005'
),
(
    '40000000-0000-4000-8000-000000000006',
    '10000000-0000-4000-8000-000000000006',
    '2026-10-18 00:00:00',
    '/certificates/NIKAH-2026-0006.pdf',
    '20000000-0000-4000-8000-000000000006'
),
(
    '40000000-0000-4000-8000-000000000007',
    '10000000-0000-4000-8000-000000000007',
    '2026-10-25 00:00:00',
    '/certificates/NIKAH-2026-0007.pdf',
    '20000000-0000-4000-8000-000000000007'
),
(
    '40000000-0000-4000-8000-000000000008',
    '10000000-0000-4000-8000-000000000008',
    '2026-11-08 00:00:00',
    '/certificates/NIKAH-2026-0008.pdf',
    '20000000-0000-4000-8000-000000000008'
),
(
    '40000000-0000-4000-8000-000000000009',
    '10000000-0000-4000-8000-000000000009',
    '2026-11-15 00:00:00',
    '/certificates/NIKAH-2026-0009.pdf',
    '20000000-0000-4000-8000-000000000009'
),
(
    '40000000-0000-4000-8000-000000000010',
    '10000000-0000-4000-8000-000000000010',
    '2026-11-22 00:00:00',
    '/certificates/NIKAH-2026-0010.pdf',
    '20000000-0000-4000-8000-000000000010'
);


-- ============================================================
-- 4. NonAhmadiGuardians
-- ============================================================

INSERT INTO NonAhmadiGuardians
(
    Id,
    FirstName,
    LastName,
    OtherName,
    PhoneNumber,
    Address,
    Signature,
    MarriageApplicationFormId,
    Religion
)
VALUES
(
    1,
    'Peter',
    'Eze',
    'Chinedu',
    '08034562345',
    'GRA, Benin City',
    '/signatures/guardian-001.png',
    '30000000-0000-4000-8000-000000000008',
    'Christianity'
),
(
    2,
    'Emmanuel',
    'Okoro',
    'Samuel',
    '08123456780',
    'Ikeja, Lagos',
    '/signatures/guardian-002.png',
    '30000000-0000-4000-8000-000000000001',
    'Christianity'
),
(
    3,
    'Joseph',
    'Adekunle',
    'Tunde',
    '08045671234',
    'Ibadan, Oyo',
    '/signatures/guardian-003.png',
    '30000000-0000-4000-8000-000000000005',
    'Christianity'
),
(
    4,
    'Michael',
    'Daniels',
    'David',
    '08156782340',
    'Wuse, Abuja',
    '/signatures/guardian-004.png',
    '30000000-0000-4000-8000-000000000003',
    'Christianity'
),
(
    5,
    'Daniel',
    'Williams',
    'Joseph',
    '08067893451',
    'Port Harcourt, Rivers',
    '/signatures/guardian-005.png',
    '30000000-0000-4000-8000-000000000006',
    'Christianity'
),
(
    6,
    'Patrick',
    'Musa',
    'John',
    '08178904562',
    'Kaduna, Kaduna',
    '/signatures/guardian-006.png',
    '30000000-0000-4000-8000-000000000007',
    'Christianity'
),
(
    7,
    'Samuel',
    'Bello',
    'Paul',
    '08089015673',
    'Kano, Kano',
    '/signatures/guardian-007.png',
    '30000000-0000-4000-8000-000000000004',
    'Christianity'
),
(
    8,
    'George',
    'Lawal',
    'Peter',
    '08190126784',
    'Ilorin, Kwara',
    '/signatures/guardian-008.png',
    '30000000-0000-4000-8000-000000000010',
    'Christianity'
),
(
    9,
    'Anthony',
    'Ibrahim',
    'Mark',
    '08011237895',
    'Yaba, Lagos',
    '/signatures/guardian-009.png',
    '30000000-0000-4000-8000-000000000009',
    'Christianity'
),
(
    10,
    'Francis',
    'Abdullahi',
    'James',
    '08122348906',
    'Garki, Abuja',
    '/signatures/guardian-010.png',
    '30000000-0000-4000-8000-000000000002',
    'Christianity'
);


-- ============================================================
-- 5. AuditLogs
-- ============================================================

INSERT INTO AuditLogs
(
    Id,
    UserId,
    Action,
    EntityName,
    RecordId,
    Timestamp,
    ChangeDetails
)
VALUES
(
    '50000000-0000-4000-8000-000000000001',
    '20000000-0000-4000-8000-000000000001',
    'Approved',
    'MarriageApplication',
    '10000000-0000-4000-8000-000000000001',
    '2026-08-23 10:15:00',
    'Marriage application approved for Nikah on 2026-09-05.'
),
(
    '50000000-0000-4000-8000-000000000002',
    '20000000-0000-4000-8000-000000000002',
    'Approved',
    'MarriageApplication',
    '10000000-0000-4000-8000-000000000002',
    '2026-08-28 11:20:00',
    'Marriage application approved for Nikah on 2026-09-12.'
),
(
    '50000000-0000-4000-8000-000000000003',
    '20000000-0000-4000-8000-000000000003',
    'Approved',
    'MarriageApplication',
    '10000000-0000-4000-8000-000000000003',
    '2026-09-01 09:30:00',
    'Marriage application approved for Nikah on 2026-09-19.'
),
(
    '50000000-0000-4000-8000-000000000004',
    '20000000-0000-4000-8000-000000000004',
    'Approved',
    'MarriageApplication',
    '10000000-0000-4000-8000-000000000004',
    '2026-09-08 14:10:00',
    'Marriage application approved for Nikah on 2026-10-03.'
),
(
    '50000000-0000-4000-8000-000000000005',
    '20000000-0000-4000-8000-000000000005',
    'Approved',
    'MarriageApplication',
    '10000000-0000-4000-8000-000000000005',
    '2026-09-15 12:45:00',
    'Marriage application approved for Nikah on 2026-10-10.'
),
(
    '50000000-0000-4000-8000-000000000006',
    '20000000-0000-4000-8000-000000000006',
    'Approved',
    'MarriageApplication',
    '10000000-0000-4000-8000-000000000006',
    '2026-09-22 13:05:00',
    'Marriage application approved for Nikah on 2026-10-17.'
),
(
    '50000000-0000-4000-8000-000000000007',
    '20000000-0000-4000-8000-000000000007',
    'Approved',
    'MarriageApplication',
    '10000000-0000-4000-8000-000000000007',
    '2026-09-29 08:50:00',
    'Marriage application approved for Nikah on 2026-10-24.'
),
(
    '50000000-0000-4000-8000-000000000008',
    '20000000-0000-4000-8000-000000000008',
    'Approved',
    'MarriageApplication',
    '10000000-0000-4000-8000-000000000008',
    '2026-10-13 15:20:00',
    'Marriage application approved for Nikah on 2026-11-07.'
),
(
    '50000000-0000-4000-8000-000000000009',
    '20000000-0000-4000-8000-000000000009',
    'Approved',
    'MarriageApplication',
    '10000000-0000-4000-8000-000000000009',
    '2026-10-20 10:40:00',
    'Marriage application approved for Nikah on 2026-11-14.'
),
(
    '50000000-0000-4000-8000-000000000010',
    '20000000-0000-4000-8000-000000000010',
    'Approved',
    'MarriageApplication',
    '10000000-0000-4000-8000-000000000010',
    '2026-10-27 16:00:00',
    'Marriage application approved for Nikah on 2026-11-21.'
);


COMMIT;