const statuses = [
   { value: 0, label: 'Nowe' },
   { value: 1, label: 'W trakcie rozpatrywania' },
   { value: 2, label: 'Zakończone' }
];

export const ReportStatusConverter = statusValue => {
  return statuses.find(x => x.value === statusValue).label
}

export const GetStatuses = () => {
  return statuses;
}